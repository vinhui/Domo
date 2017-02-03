require File.dirname(__FILE__) + '/../../spec_helper'

describe "IO::popen" do
  # TODO: rewrite to not depend on platform specific utilities
  # NOTE: cause Errno::EBADF on 1.8.6
  #ruby_bug "#", "1.8.6" do
    it "reads from a read-only pipe" do
      IO.popen("echo foo", "r") do |io|
        io.read.should == "foo\n"

        lambda { io.write('foo').should }.should \
          raise_error(IOError, 'not opened for writing')
      end
    end

    platform_is_not :windows do
      it "reads and writes to a read/write pipe" do
        data = IO.popen("cat", "r+") do |io|
          io.write("bar")
          io.read 3
        end

        data.should == "bar"
      end

      it "writes to a write-only pipe" do
        begin
          tmp_file = tmp "IO_popen_spec_#{$$}"

          data = IO.popen "cat > #{tmp_file}", 'w' do |io|
            io.write 'bar'

            lambda { io.read.should }.should \
              raise_error(IOError, 'not opened for reading')
          end
          system 'sync' # sync to flush writes for File.read below

          File.read(tmp_file).should == 'bar'

        ensure
          File.unlink tmp_file if File.exist? tmp_file
        end
      end
    end

    platform_is :windows do
      it "allows the io to be closed inside the block" do
      
        io = IO.popen('dir', 'r') do |io|
          io.close

          io.closed?.should == true

          io
        end

        io.closed?.should == true
      end
    end
    
    platform_is_not :windows do
      it "allows the io to be closed inside the block" do
      
        io = IO.popen('ls', 'r') do |io|
          io.close

          io.closed?.should == true

          io
        end

        io.closed?.should == true
      end
    end
  #end
end

