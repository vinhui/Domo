require File.dirname(__FILE__) + '/../../spec_helper'
require File.dirname(__FILE__) + '/fixtures/common'
require File.dirname(__FILE__) + '/shared/closed'
require File.dirname(__FILE__) + '/shared/pos'

describe "Dir#pos" do
  before :all do
    DirSpecs.create_mock_dirs
  end

  after :all do
    DirSpecs.delete_mock_dirs
  end

  it_behaves_like :dir_pos, :pos
end

describe "Dir#pos" do
  before :all do
    DirSpecs.create_mock_dirs
  end

  after :all do
    DirSpecs.delete_mock_dirs
  end

  it_behaves_like :dir_closed, :pos
end

describe "Dir#pos=" do
  before :all do
    DirSpecs.create_mock_dirs
  end

  after :all do
    DirSpecs.delete_mock_dirs
  end

  it_behaves_like :dir_pos_set, :pos=
end
