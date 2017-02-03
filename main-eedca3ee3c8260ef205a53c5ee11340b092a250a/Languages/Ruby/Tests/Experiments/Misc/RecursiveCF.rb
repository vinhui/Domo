def foo
  puts 'foo-begin'
  bar { puts 'block'; break }
  puts 'foo-end'
end

def bar &p
  puts 'bar-begin'
  $g = p
  puts 'bar-end'
end

def baz &p
  puts 'baz-begin'
  yield
  puts 'baz-end'
end
 
foo
baz &$g