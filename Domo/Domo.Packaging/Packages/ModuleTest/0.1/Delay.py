from System.ComponentModel import BackgroundWorker

class Delay():
	delay = 0

	def __init__(self):
		pass

	def __init__(self, callback, time):
		self.delay = time

		self.worker = BackgroundWorker()
		self.worker.DoWork += self.doTimer
		self.worker.RunWorkerCompleted += lambda *a : callback()
		self.worker.RunWorkerAsync()
		pass

	def doTimer(self, sender, args):
		Thread.Sleep(self.delay)
		pass
