require File.dirname(__FILE__) + '/../../spec_helper'

$require_fixture_dir = (File.dirname(__FILE__) + '/../../fixtures/require')
$require_tmp_dir = tmp("require_specs")
$LOAD_PATH << $require_fixture_dir
$LOAD_PATH << $require_tmp_dir


require 'rbconfig'

describe "Kernel#require" do
  conflicts_with :Gem do
    # rubygems redefines #require without setting its
    # visibility back to module_function or private
    it "is a private method" do
      Kernel.should have_private_instance_method(:require)
    end
  end

  # Avoid storing .rbc in repo
  before :each do
    # We explicitly delete and recreate our temporary directories every time
    # to avoid establishing dependencies between tests. 
    rm_r $require_tmp_dir
    Dir.mkdir($require_tmp_dir)
    Dir.chdir($require_tmp_dir) { 
      touch "require_spec_dummy.#{Config::CONFIG['DLEXT']}"
      touch "require_spec_dummy.rb"
    }
    $LOADED_FEATURES.delete_if {|path| path =~ /require_spec/}
    $require_spec   = nil
    $require_spec_1 = nil
    $require_spec_2 = nil
    $require_spec_3 = nil
    $require_spec_4 = nil
    $require_spec_5 = nil
    $require_spec_6 = nil
    $require_spec_7 = nil
    $require_spec_8 = nil
    $require_spec_9 = nil
    $require_spec_10 = nil
    $require_spec_rooby = nil
    $require_spec_recursive = nil
  end

  after :each do
    rm_r $require_tmp_dir
  end

  # The files used below just contain code that assigns
  # Time.now to the respective global variable so that
  # reloads can easily be verified.

  # CAUTION: Some of these work because a different path is used to
  #          load the same file. Be careful if you change the order
  #          or add items.

#######################################
# PATH RESOLUTION
#######################################
  
  it "resolves paths relative to the current working directory" do
    Dir.chdir($require_fixture_dir) do |dir|
      require('../../fixtures/require/require_spec_1.rb').should be_true
      $require_spec_1.should_not be_nil
      $LOADED_FEATURES.grep(/require_spec_1\.rb/).size.should == 1
    end
  end

  ruby_version_is "1.9" do
    # This expectation is necessarily long-winded because the conditions are
    # particularly specific. Namely:
    #   
    #   * The directory containing the file isn't in $LOAD_PATH
    #   * The filename has no path components prepended
    #   * The file hasn't already been required
    #   * The file exists
    #
    # For reference see [ruby-core:24155] in which matz confirms this feature is
    # intentional for security reasons. 
    it "does not resolve completely unqualified filenames against the current working directory unless it appears in $LOAD_PATH" do
      dir = File.join($require_tmp_dir, $$.to_s)
      $LOAD_PATH.include?(dir).should be_false
      File.directory?(dir).should be_false
      Dir.mkdir(dir)
      File.directory?(dir).should be_true
      Dir.chdir(dir) do |tmp_dir|
        file = "#{$$}_#{Process.times.utime}.rb"
        touch file
        File.exist?(file).should be_true
        lambda { require file }.should raise_error(LoadError)
        File.unlink(file)
      end
      Dir.rmdir(dir)
    end
  end

  it "does not expand/resolve qualified files against $LOAD_PATH" do
    Dir.chdir($require_fixture_dir + '/../') do |dir|
      # This would be a valid path if expanded against the fixture dir
      lambda { require '../require/require_spec_2.rb' }.should raise_error LoadError
  
      # Rubygems delayed loading might have kicked in, so count features now:
      num_features = $LOADED_FEATURES.size
      lambda { require '../require/require_spec_2.rb' }.should raise_error LoadError
      $LOADED_FEATURES.size.should == num_features
    end
  end

  it "loads a .rb from an absolute path" do
    path = File.join($require_fixture_dir, 'require_spec_1.rb')
    require(path).should be_true
    $require_spec_1.should_not be_nil
  end

  it "collapses consecutive path separators" do
    abs_path = File.join($require_fixture_dir, '/require_spec_1.rb')
    path_parts = File.split(abs_path)
    # Mangle the absolute path so it contains multiple consecutive separator
    # characters, then require it.
    require([
      path_parts[0], File::Separator, path_parts[1]
    ].join(File::Separator)).should be_true
    $require_spec_1.should_not be_nil
  end

  # Has been fixed on 1.9.2 HEAD; not backported yet
  ruby_bug "#1627", "1.9.2" do
    ruby_version_is "1.9" do
      it "collapses '../' inside an absolute path" do
        abs_path = File.expand_path(File.join($require_fixture_dir, 'require_spec_1.rb'))
        
        require(abs_path).should be_true
        
        # insert "xxx/.." in the middle of the path:
        dir, file = File.split(abs_path)
        require File.join(dir, 'xxx', '..', file)
                
        $LOADED_FEATURES.grep(/require_spec_1\.rb/).should == [abs_path]
      end
    end
  end

  it "loads an unqualified .rb by looking in $LOAD_PATH and returning true" do
    require('require_spec_2.rb').should be_true
    $require_spec_2.should_not be_nil
  end

  it "allows unqualified files to contain path information (just not in the beginning)" do
    name = (File.dirname(__FILE__) + '/../../fixtures')
    $LOAD_PATH << name
    require('require/../require/require_spec_2.rb').should be_true
    $require_spec_2.should_not be_nil      
  end

  it "loads a file with ./filename even if . is not in path" do
    Dir.chdir($require_fixture_dir) do |dir| 
	    path_backup = $LOAD_PATH.clone
	    $LOAD_PATH.clear
	    $LOAD_PATH << "Someirrelevantpath"
      begin
        require('./require_spec.rb').should be_true    
      ensure
	      $LOAD_PATH.clear
	      $LOAD_PATH.concat(path_backup)    
	    end
    end 
	end

#######################################
# PATH CANONICALIZATION
#######################################

  # This bug has been fixed on 1.9.2 HEAD.
  ruby_bug "#1627", "1.9.2" do
    ruby_version_is "1.9" do
      it "stores relative paths as absolute paths in $LOADED_FEATURES" do
        Dir.chdir($require_fixture_dir) do |dir|
          abs_path = File.expand_path('../require/require_spec_1.rb')
          require(abs_path).should be_true
          require("../require/require_spec_1.rb").should be_false
          $LOADED_FEATURES.grep(/require_spec_1\.rb/).should == [abs_path]
        end
      end
      
      it "stores ./file paths as absolute paths in $LOADED_FEATURES" do
        Dir.chdir($require_fixture_dir) do |dir|
          abs_path = File.expand_path('./require_spec_1.rb')
          require(abs_path).should be_true
          require("./require_spec_1.rb").should be_false
          $LOADED_FEATURES.grep(/require_spec_1\.rb/).should == [abs_path]
        end
      end

      it "performs tilde expansion before storing paths in $LOADED_FEATURES" do
        begin
          original_home = ENV['HOME'].dup
          ENV['HOME'] = $require_fixture_dir
          abs_path = File.expand_path(
            File.join($require_fixture_dir, 'require_spec_1.rb'))
          tilde_path = File.join('~', 'require_spec_1.rb')
          require(tilde_path).should be_true
          $LOADED_FEATURES.grep(/require_spec_1\.rb/).should == [abs_path]
        ensure
          ENV['HOME'] = original_home
        end  
      end

      it "collapses multiple consecutive path separators before storing in $LOADED_FEATURES" do
        Dir.chdir($require_fixture_dir) do |dir|
          abs_path = File.expand_path('../require/require_spec_1.rb')
          path_parts = File.split(abs_path)
          require([
            path_parts[0], File::Separator, path_parts[1]
          ].join(File::Separator)).should be_true

          $LOADED_FEATURES.grep(/require_spec_1\.rb/).should == [abs_path]
        end
      end

      it "collapses '../' inside an absolute path before storing in $LOADED_FEATURES" do
        abs_path = File.expand_path(File.join($require_fixture_dir, '/require_spec_1.rb'))
        
        require(abs_path).should be_true
        
        # insert "xxx/.." in the middle of the path:
        dir, file = File.split(abs_path)
        require File.join(dir, 'xxx', '..', file)
                
        $LOADED_FEATURES.grep(/require_spec_1\.rb/).should == [abs_path]
      end
    end    
  end

  it "stores a non-extensioned file with its located suffix" do
    require('require_spec_6').should be_true
    $LOADED_FEATURES.grep(/require_spec_6\.rb/).should_not == []
  end
 
#######################################
# FILE EXTENSIONS
#######################################
  
  it "appends a file with no extension with .rb/.<ext> in that order to locate file" do
    load('require_spec')
    $require_spec.should == :noext
    load('require_spec.rb')
    $require_spec.should == :rb

    $require_spec = nil

    # 1.9 won't re-require 'require_spec.rb' because it already appears in
    # $LOADED_FEATURES, so we delete it first. On 1.8 this has no effect.
    $LOADED_FEATURES.delete File.expand_path(
      File.join($require_fixture_dir, 'require_spec.rb'))
    require('require_spec')
    $require_spec.should == :rb
  end

  it "prefers to use .rb over .<ext> if given non-extensioned file and both exist" do
    require('require_spec_dummy').should be_true
    $LOADED_FEATURES.grep(/require_spec_dummy\.rb/).should_not == []
    $LOADED_FEATURES.grep(/require_spec_dummy\.#{Config::CONFIG['DLEXT']}/).should == []
  end

  it "will load file.rb when given 'file' if it exists even if file.<ext> is loaded" do
    $LOADED_FEATURES << "require_spec_3.#{Config::CONFIG['DLEXT']}"
    require('require_spec_3.rb').should be_true
    $LOADED_FEATURES.grep(/require_spec_3\.rb/).should_not == []
  end

  it "will not load file.<ext> when given 'file' if file.rb already loaded" do
    require('require_spec_dummy.rb').should be_true
    require('require_spec_dummy').should be_false
  end

  it "appends any non-ruby extensioned file with .rb/.<ext> in that order to locate file" do
    load('require_spec.rooby')
    $require_spec_rooby.should == :rooby
    load('require_spec.rooby.rb')
    $require_spec_rooby.should == :rb

    $require_spec_rooby = nil

    require('require_spec.rooby')
    $require_spec_rooby.should == :rb
  end

  # TODO: add an implementation-agnostic method for creating
  # an extension file
  it "loads extension files"

  # TODO: add an implementation-agnostic method for creating
  # an extension file
  it "will load explicit file.<ext> even if file.rb already loaded and vice versa"


#######################################
# $LOADED_FEATURES
#######################################

  it "stores the loaded file in $LOADED_FEATURES" do
    require('require_spec_6.rb').should be_true
    $LOADED_FEATURES.grep(/require_spec_6\.rb/).should_not == []
  end

  runner_is_not :rspec do
    it "will not add a bad load to LOADED_FEATURES" do
      lambda { require('require_spec_raises') }.should raise_error(RuntimeError)
    
      $LOADED_FEATURES.grep(/require_spec_raises\.rb/).should == []
    end
  end


  it "uses $LOADED_FEATURES to see whether file is already loaded" do
    require('require_spec_7.rb').should be_true

    $LOADED_FEATURES.delete_if {|path| path =~ /require_spec_7\.rb$/}
    require('require_spec_7.rb').should be_true
    require('require_spec_7.rb').should be_false

    $LOADED_FEATURES.grep(/require_spec_7\.rb/).should_not == []
  end

  it "will not load a file whose path appears in $LOADED_FEATURES; it will return false" do
    # This wording is necessarily precise because 1.8 and 1.9 perform
    # different forms of normalisation before storing the path in
    # $LOADED_FEATURES. We deliberately don't concern ourselves with the form
    # of the saved path; just that an approximation of it has been recorded.
    require('require_spec_7.rb').should be_true
    a = $require_spec_7
    a.should_not be_nil
    
    $LOADED_FEATURES.grep(/require_spec_7.rb/).should_not == []

    require('require_spec_7.rb').should be_false
    b = $require_spec_7
    b.should_not be_nil

    # Timestamps should not differ
    a.should eql(b)

    $LOADED_FEATURES.grep(/require_spec_7\.rb/).should_not == []
  end

#######################################
# ERROR BEHAVIOUR
#######################################

  it "requires arbitrarily complex files (files with large numbers of AST nodes)" do
    lambda {require File.expand_path(File.dirname(__FILE__)) + '/fixtures/test'}.should_not raise_error
  end

  it "raises a LoadError if the file can't be found" do
    lambda { require "nonesuch#{$$}#{Time.now.to_f}" }.should raise_error LoadError
  end

  platform_is_not :os => [:windows, :cygwin] do # can't make file unreadable
    it "raises a LoadError if the file exists but can't be read" do
      abs_path = File.expand_path(File.join($require_tmp_dir, 'require_spec_dummy.rb'))
      File.exists?(abs_path).should be_true
      file = File.new(abs_path)
      begin
        file.chmod(0000)
        lambda {require(abs_path)}.should raise_error(LoadError)
      ensure
        file.close
      end
    end
  end

  ruby_version_is ""..."1.9" do
    it "only accepts strings and objects with #to_str" do
      lambda { require(nil) }.should raise_error(TypeError)
      lambda { require(42)  }.should raise_error(TypeError)
      lambda { require([])  }.should raise_error(TypeError)

      # objects with to_s are not good enough
      o = mock('require_spec_dummy');
      o.should_receive(:to_s).any_number_of_times.and_return("require_spec_dummy")
      lambda { require(o) }.should raise_error(TypeError)

      # objects with to_path are not good enough
      o = mock('require_spec_dummy');
      o.should_receive(:to_path).any_number_of_times.and_return("require_spec_dummy")
      lambda { require(o) }.should raise_error(TypeError)
    end
  end

  ruby_version_is "1.9" do
    it "only accepts string or objects with #to_path or #to_str" do
      lambda { require(nil) }.should raise_error(TypeError)
      lambda { require(42)  }.should raise_error(TypeError)
      lambda { require([])  }.should raise_error(TypeError)
    end

    it "calls #to_path on non-String arguments" do
      abs_path = File.expand_path(
        File.join($require_fixture_dir, 'require_spec.rb'))
      path = mock('abs_path')
      path.should_receive(:to_path).and_return(abs_path)
      require(path).should be_true
      $LOADED_FEATURES.include?(abs_path).should be_true
    end

    it "does not call #to_path on String arguments" do
      abs_path = File.expand_path(
        File.join($require_fixture_dir, 'require_spec.rb'))

      def abs_path.to_path
        'blah-non-existing-path'
      end
      require(abs_path).should be_true
      $LOADED_FEATURES.include?(abs_path).should be_true
    end

    it "calls #to_str on non-String objects returned by #to_path" do
      abs_path = File.expand_path(
        File.join($require_fixture_dir, 'require_spec.rb'))

      non_string_path = mock("non_string_path")
      non_string_path.should_receive(:to_str).and_return(abs_path)

      path = mock('path')
      path.should_receive(:to_path).and_return(non_string_path)

      require(path).should be_true
      $LOADED_FEATURES.include?(abs_path).should be_true
    end
  end

  it "calls #to_str on non-String arguments" do
    abs_path = File.expand_path(
        File.join($require_fixture_dir, 'require_spec.rb'))
    o = mock('require_spec');
    o.should_receive(:to_str).and_return(abs_path)
    $LOADED_FEATURES.include?(abs_path).should be_false
    require(o).should be_true
    $LOADED_FEATURES.include?(abs_path).should be_true

    nil_mock = mock('nil')
    nil_mock.should_receive(:to_str).at_least(1).and_return(nil)
    lambda { require(nil_mock) }.should raise_error(TypeError)
  end

  it "does not infinite loop on an rb file that requires itself" do
    $LOADED_FEATURES.grep(/require_spec_recursive\.rb/).should == []
    require('require_spec_recursive').should be_true
    $LOADED_FEATURES.grep(/require_spec_recursive.rb/).should_not == []
    $require_spec_recursive.should_not be_false
  end

#######################################
# __FILE__ / __LINE__
#######################################

  ruby_version_is ""..."1.9" do
    it "produces __FILE__ as the given filename and __LINE__ as the source line number" do
      Dir.chdir($require_fixture_dir) do |dir|
        require('require_spec_4').should be_true 
        $require_spec_4.should == [['./require_spec_4.rb', 1], ['./require_spec_4.rb', 10]]

        extended_on :rubinius do
          `rm require_spec_4.rbc`
        end
      end

      $require_spec_4 = nil

      require("#{$require_fixture_dir}/require_spec_4").should be_true 
      $require_spec_4[0][0].should =~ %r[^.*/fixtures/require/require_spec_4.rb]
      $require_spec_4[0][1].should == 1
      $require_spec_4[1][0].should =~ %r[^.*/fixtures/require/require_spec_4.rb]
      $require_spec_4[1][1].should == 10
    end
  end

  ruby_version_is "1.9" do
    it "produces __FILE__ as the given filename and __LINE__ as the source line number" do
      abs_path = File.expand_path(
        File.join($require_fixture_dir, 'require_spec_4.rb'))
      Dir.chdir($require_fixture_dir) do |dir|
        require('require_spec_4').should be_true 
        $require_spec_4.should == [[abs_path, 1], [abs_path, 10]]

        extended_on :rubinius do
          `rm require_spec_4.rbc`
        end
      end

      $require_spec_4 = nil
      $LOADED_FEATURES.delete abs_path
      require('require_spec_4').should be_true 
      $require_spec_4[0][0].should =~ %r[^.*/fixtures/require/require_spec_4.rb]
      $require_spec_4[0][1].should == 1
      $require_spec_4[1][0].should =~ %r[^.*/fixtures/require/require_spec_4.rb]
      $require_spec_4[1][1].should == 10
    end
  end
end

describe "Shell expansion in Kernel#require" do
  before :all do
    @rs_home = ENV["HOME"]
    ENV["HOME"] = $require_fixture_dir
    @rs_short = "~/require_spec_1.rb"
    @rs_long  = "#{$require_fixture_dir}/require_spec_1.rb"
    @rs_abs = File.expand_path(@rs_long)
  end

  after :all do
    ENV["HOME"] = @rs_home
  end

  before :each do
    [@rs_long, @rs_short, @rs_abs].each do |path|
      $LOADED_FEATURES.delete path
    end      
  end

  it "expands a preceding ~/ to the user's home directory for building the path to search" do
    $require_spec_1 = nil
    require(@rs_short).should == true
    $require_spec_1.nil?.should == false
  end

  it "adds the path to $LOADED_FEATURES" do
    $require_spec_1 = nil
    require(@rs_short).should == true
    $require_spec_1.nil?.should == false

    $LOADED_FEATURES.find {|f| 
      f == @rs_short || f == @rs_long || f == @rs_abs
    }.nil?.should == false
  end
end

describe "Kernel.require" do
  it "needs to be reviewed for spec completeness"
end
