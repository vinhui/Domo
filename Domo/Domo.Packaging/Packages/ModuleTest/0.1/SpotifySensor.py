from Domo.Modules import *

class SpotifySensor(SensorModule[SpotifyInterface]):
	status = None

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