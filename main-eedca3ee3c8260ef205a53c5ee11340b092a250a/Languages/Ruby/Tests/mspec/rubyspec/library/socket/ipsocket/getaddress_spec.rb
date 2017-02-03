require File.dirname(__FILE__) + '/../../../spec_helper'
require File.dirname(__FILE__) + '/../fixtures/classes'

describe "Socket::IPSocket#getaddress" do

  it "returns the IP address of hostname" do
    roundtrip_hostname = Socket.getaddrinfo(IPSocket.getaddress(SocketSpecs.hostname), nil)[0][2]
    roundtrip_hostname.should == SocketSpecs.hostname
  end

  it "returns the IP address when passed an IP" do
    IPSocket.getaddress("127.0.0.1").should == "127.0.0.1"
    IPSocket.getaddress("0.0.0.0").should == "0.0.0.0"
  end

  it "raises an error on unknown hostnames" do
    lambda { IPSocket.getaddress("imfakeidontexistanditrynottobeslow.com") }.should raise_error(SocketError)
  end

end
