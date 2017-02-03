require File.dirname(__FILE__) + '/../../../spec_helper'

describe "File::Stat#mode" do
  before :each do
    @file = tmp('i_exist')
    touch(@file) { |f| f.write "rubinius" }
    File.chmod(0755, @file)
  end

  after :each do
    rm_r @file
  end
  
  it "should be able to determine the mode through a File::Stat object" do
    st = File.stat(@file)
    st.mode.is_a?(Integer).should == true
    platform_is_not :windows do
      st.mode.should == 33261
    end
    platform_is :windows do
      st.mode.should == 33188
    end
  end
end
