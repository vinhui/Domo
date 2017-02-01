from Domo.Modules import *

from System.Threading import Thread, ThreadStart

class LanSensor(SensorModule):
	devices = []
	onScanDone = []
	
	portScanner = None
	scanHosts = "192.168.0.0/24"
	#scanArgs = "-n -sP -T4"
	scanArgs = "-n -sP"

	scanningThread = None

	scanInterval = 1000

	def OnEnable(self):
		self.portScanner = PortScanner()

		self.scanningThread = None
		self.scanningThread = Thread(ThreadStart(self.scanningLoop))
		self.scanningThread.Start()
		pass

	def OnDisable(self):
		if self.scanningThread is not None:
			self.scanningThread.Abort()

		self.portScanner = None
		pass

	def registerCallback(self, call):
		self.onScanDone.append(call)
		pass

	def unregisterCallback(self, call):
		self.onScanDone.remove(call)
		pass

	def scanningLoop(self):
		if self.portScanner is not None:
			self.devices = []
			result = self.portScanner.scan(hosts=self.scanHosts, arguments=self.scanArgs)
			Log.Debug("Nmap scan cycle done")
			if result is not None:
				for ip, obj in result["scan"].iteritems():
					if "mac" in obj["addresses"]:
						self.devices.append(LanDevice(obj))

			for i in self.onScanDone:
				i()

			Thread.Sleep(self.scanInterval)
			self.scanningLoop()
		pass

class LanDevice:
	ip = ""
	mac = ""
	vendor = ""

	def __init__(self):
		pass

	def __init__(self, nmapObj):
		self.ip = nmapObj["addresses"]["ipv4"]
		self.mac = nmapObj["addresses"]["mac"]

		if len(nmapObj["vendor"]) > 0:
			self.vendor = nmapObj["vendor"][self.mac]
		pass