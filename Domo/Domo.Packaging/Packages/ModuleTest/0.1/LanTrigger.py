from Domo.Modules import *

class LanTrigger(SensorTriggerModule):
	triggerOnFirst = True
	
	knownDevices = []
	triggerDevices = ["C0:EE:FB:ED:6B:98"]

	joinedDevices = []
	leftDevices = []

	onDeviceJoined = []
	onDeviceLeft = []

	def __init__(self):
		self.onDeviceJoined.append(self.deviceJoined)
		self.onDeviceLeft.append(self.deviceLeft)

		if not self.triggerOnFirst:
			self.isFirstRun = True
		pass

	def OnEnable(self):
		SensorTriggerModule.init(self, LanSensor)
		self.sensor.registerCallback(self.onListUpdated)
		pass

	def OnDisable(self):
		pass

	def onListUpdated(self):
		self.compareDevices(self.knownDevices, self.sensor.devices)
		self.knownDevices = self.sensor.devices

		if not self.triggerOnFirst:
			if self.isFirstRun:
				self.isFirstRun = False
				return

		self.eventCheck(self.joinedDevices, self.triggerDevices, self.onDeviceJoined)
		self.eventCheck(self.leftDevices, self.triggerDevices, self.onDeviceLeft)
		pass

	def eventCheck(self, devices, triggerDevices, callbacks):
		for device in devices:
			if len(triggerDevices) > 0 and device.mac not in triggerDevices:
				continue

			self.OnTrigger()
			for c in callbacks:
				c(device)
		pass

	def compareDevices(self, oldList, newList):
		self.joinedDevices = []
		self.leftDevices = []

		newListMacs = [x.mac for x in newList]
		oldListMacs = [x.mac for x in oldList]

		for i, d in enumerate(newListMacs):
			if d not in oldListMacs:
				self.joinedDevices.append(newList[i])

		for i, d in enumerate(oldListMacs):
			if d not in newListMacs:
				self.leftDevices.append(oldList[i])
		pass

	def deviceJoined(self, device):
		Log.Info("A lan device joined ({0})", device.mac)
		pass

	def deviceLeft(self, device):
		Log.Info("A lan device left ({0})", device.mac)
		pass