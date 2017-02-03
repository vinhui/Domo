require File.dirname(__FILE__) + '/../../spec_helper'
require File.dirname(__FILE__) + '/fixtures/classes'
require File.dirname(__FILE__) + '/shared/enumeratorize'

describe "Array#delete_if" do
  before do
    @a = [ "a", "b", "c" ]
  end

  it "removes each element for which block returns true" do
    @a.delete_if { |x| x >= "b" }
    @a.should == ["a"]
  end

  it "returns self" do
    @a.delete_if{ true }.equal?(@a).should be_true
  end

  it_behaves_like :enumeratorize, :delete_if

  ruby_version_is '1.8.7' do
    it "returns an Enumerator if no block given, and the enumerator can modify the original array" do
      enum = @a.delete_if
      enum.should be_kind_of(enumerator_class)
      @a.should_not be_empty
      enum.each { true }
      @a.should be_empty
    end
  end

  ruby_version_is '' ... '1.9' do
    it "raises a TypeError on a frozen array" do
      lambda { ArraySpecs.frozen_array.delete_if {} }.should raise_error(TypeError)
    end
  end

  ruby_version_is '1.9' do
    it "raises a RuntimeError on a frozen array" do
      lambda { ArraySpecs.frozen_array.delete_if {} }.should raise_error(RuntimeError)
    end
  end

  it "keeps tainted status" do
    @a.taint
    @a.tainted?.should be_true
    @a.delete_if{ true }
    @a.tainted?.should be_true
  end

  ruby_version_is '1.9' do
    it "keeps untrusted status" do
      @a.untrust
      @a.untrusted?.should be_true
      @a.delete_if{ true }
      @a.untrusted?.should be_true
    end
  end
  
  #see http://ironruby.codeplex.com/WorkItem/View.aspx?WorkItemId=3314
  it "doesn't cache deletes incorrectly" do
    @a.delete_if {|x| x == "b"}
    @a.should == ["a","c"]
    Array.new(@a).should == ["a","c"]
  end
end
