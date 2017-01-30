from Domo.Modules import *

class SpotifySensor(SensorModule):
	status = None

	def __init__(self):
		SensorModule.init(self, SpotifyInterface)
		pass
	def OnEnable(self):
		print(self.getStatus())
		pass

	def OnDisable(self):
		pass

	def getStatus(self):
		if self.hardwareInterface.isInitialized:
			self.status = self.hardwareInterface.get("/remote/status.json")
			return self.status
		pass