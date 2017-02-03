require File.dirname(__FILE__) + '/../../../spec_helper'

describe "File::Stat#blksize" do
  before :each do
    @file = tmp('i_exist')
    touch(@file) { |f| f.write "rubinius" }
  end

  after :each do
    rm_r @file
  end
  
  platform_is_not :windows do
    it "should be able to determine the blksize on a File::Stat object" do
      st = File.stat(@file)
      st.blksize.is_a?(Integer).should == true
      st.blksize.should > 0
    end
  end

  platform_is :windows do
    it "returns nil" do
      File.stat(@file).blksize.should be_nil
    end
  end
end
