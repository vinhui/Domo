from Domo.Modules import *

class SpotifyZController(ControllerModule):
	def __init__(self):
		ControllerModule.init(self, SpotifyInterface)
		pass

	def OnEnable(self):
		pass

	def OnDisable(self):
		pass

	def play(self):
		self.play(None)
		pass

	def play(self, track=None):
		if not self.hardwareInterface.isInitialized:
			return

		if track is None:
			self.unpause()
			pass
		else:
			self.hardwareInterface.get("/remote/play.json", {"uri": track, "context": track})
			pass
		pass

	def resume(self):
		self.unpause()
		pass

	def unpause(self):
		self.pause(False)
		pass

	def pause(self, state=True):
		if self.hardwareInterface.isInitialized:
			self.hardwareInterface.get("/remote/pause.json", {"pause": str(state).lower()})
		pass

	def next(self):
		pass