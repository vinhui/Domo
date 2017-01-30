from Domo.Modules import *

from System.Threading import Thread, ThreadStart

class SpotifySensor(SensorModule):
	status = None

	updateInterval = 5000
	
	def __init__(self):
		SensorModule.init(self, SpotifyInterface)
		pass

	def OnEnable(self):
		self.updateThread = None
		self.updateThread = Thread(ThreadStart(self.updateLoop))
		self.updateThread.Start()
		pass

	def OnDisable(self):
		if self.updateThread is not None:
			self.updateThread.Abort()
		pass

	@property
	def isPlaying(self):
		if self.status is not None and "playing" in self.status:
			return self.status["playing"]
		else:
			return False
		pass

	@property
	def artist(self):
		if self.status is not None and "track" in self.status and "artist_resource" in self.status["track"]:
			return self.status["track"]["artist_resource"]["name"]
		else:
			return None
		pass

	@property
	def track(self):
		if self.status is not None and "track" in self.status and "track_resource" in self.status["track"]:
			return self.status["track"]["track_resource"]["name"]
		else:
			return None
		pass

	def updateLoop(self):
		while True:
			self.updateStatus()
			Thread.Sleep(self.updateInterval)
		pass

	def updateStatus(self):
		if self.hardwareInterface.isInitialized:
			self.status = self.hardwareInterface.get("/remote/status.json")
			return self.status
		pass