require File.dirname(__FILE__) + '/../../spec_helper'
require File.dirname(__FILE__) + '/fixtures/classes'

class DefineMethodSpecClass
end

describe "Module#define_method when given an UnboundMethod" do
  it "correctly passes given arguments to the new method" do
    klass = Class.new do
      def test_method(arg1, arg2)
        [arg1, arg2]
      end
      define_method(:another_test_method, instance_method(:test_method))
    end

    klass.new.another_test_method(1, 2).should == [1, 2]
  end

  it "adds the new method to the methods list" do
    klass = Class.new do
      def test_method(arg1, arg2)
        [arg1, arg2]
      end
      define_method(:another_test_method, instance_method(:test_method))
    end
    klass.new.should have_method(:another_test_method)
  end

  it "can add an initializer" do
    klass = Class.new do
      attr_reader :args
      define_method(:initialize) do |*args|
        @args = args
      end
    end
    klass.new(1,2,3).args.should == [1,2,3]
  end

  ruby_bug "redmine:2117", "1.8.7" do
    it "works for singleton classes too" do
      klass = Class.new
      class << klass
        def test_method
          :foo
        end
      end
      child = Class.new(klass)
      sc = class << child; self; end
      sc.send :define_method, :another_test_method, klass.method(:test_method).unbind
      child.another_test_method.should == :foo
    end
  end
end

describe "Module#define_method" do
  it "defines the given method as an instance method with the given name in self" do
    class DefineMethodSpecClass
      def test1
        "test"
      end
      define_method(:another_test, instance_method(:test1))
    end

    o = DefineMethodSpecClass.new
    o.test1.should == o.another_test
  end

  it "defines a new method with the given name and the given block as body in self" do
    class DefineMethodSpecClass
      define_method(:block_test1) { self }
      define_method(:block_test2, &lambda { self })
    end

    o = DefineMethodSpecClass.new
    o.block_test1.should == o
    o.block_test2.should == o
  end

  it "raises a TypeError when the given method is no Method/Proc" do
    lambda {
      Class.new { define_method(:test, "self") }
    }.should raise_error(TypeError)

    lambda {
      Class.new { define_method(:test, 1234) }
    }.should raise_error(TypeError)
  end

  it "accepts a Method (still bound)" do
    class DefineMethodSpecClass
      attr_accessor :data
      def inspect_data
        "data is #{@data}"
      end
    end
    o = DefineMethodSpecClass.new
    o.data = :foo
    m = o.method(:inspect_data)
    m.should be_an_instance_of(Method)
    klass = Class.new(DefineMethodSpecClass)
    klass.send(:define_method,:other_inspect, m)
    c = klass.new
    c.data = :bar
    c.other_inspect.should == "data is bar"
    lambda{o.other_inspect}.should raise_error(NoMethodError)
  end

  it "maintains the Proc's scope" do
    class DefineMethodByProcClass
      in_scope = true
      method_proc = proc { in_scope }

      define_method(:proc_test, &method_proc)
    end

    o = DefineMethodByProcClass.new
    o.proc_test.should be_true
  end
end
