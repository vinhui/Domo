require File.dirname(__FILE__) + '/../../../spec_helper'
require File.dirname(__FILE__) + '/../fixtures/classes'

include Socket::Constants
describe :socket_bind, :shared => true do
  it "binds to a port" do
    lambda { @sock.bind(@sockaddr) }.should_not raise_error
  end

  it "returns 0 if successful" do
    @sock.bind(@sockaddr).should == 0
  end

  it "raises Errno::EINVAL when binding to an already bound port" do
    @sock.bind(@sockaddr);

    lambda { @sock.bind(@sockaddr); }.should raise_error(Errno::EINVAL);
  end

  platform_is_not :windows do
    it "raises Errno::EADDRNOTAVAIL when the specified sockaddr is not available from the local machine" do
      sockaddr1 = Socket.pack_sockaddr_in(SocketSpecs.port, "4.3.2.1");
      lambda { @sock.bind(sockaddr1); }.should raise_error(Errno::EADDRNOTAVAIL)
    end
  end

  platform_is :windows do
    it "raises Errno::ENETDOWN when the specified sockaddr is not available from the local machine" do
      sockaddr1 = Socket.pack_sockaddr_in(SocketSpecs.port, "4.3.2.1");
      lambda { @sock.bind(sockaddr1); }.should raise_error(Errno::ENETDOWN)
    end
  end
  
  platform_is_not :os => [:windows, :cygwin] do
    it "raises Errno::EACCES when the current user does not have permission to bind" do
      sockaddr1 = Socket.pack_sockaddr_in(1, "127.0.0.1");
      lambda { @sock.bind(sockaddr1); }.should raise_error(Errno::EACCES)
    end
  end
end

describe "Socket#bind on SOCK_DGRAM socket" do
  before :each do
    @sock = Socket.new(AF_INET, SOCK_DGRAM, 0);
    @sockaddr = Socket.pack_sockaddr_in(SocketSpecs.port, "127.0.0.1");
  end

  after :each do
    @sock.closed?.should be_false
    @sock.close
  end                           
  it_behaves_like :socket_bind, nil
end

describe "Socket#bind on SOCK_STREAM socket" do
  before :each do
    @sock = Socket.new(AF_INET, SOCK_STREAM, 0);
    @sockaddr = Socket.pack_sockaddr_in(SocketSpecs.port, "127.0.0.1");
  end

  after :each do
    @sock.closed?.should be_false
    @sock.close
  end
  it_behaves_like :socket_bind, nil
end
