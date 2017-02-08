from Domo.Modules import *
from Domo.API import ApiManager, ApiResponse

from System.Collections.Generic import Dictionary
from System.Threading import Thread, ThreadStart

from nmap import PortScanner, PortScannerError

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
		try:
			self.portScanner = PortScanner()
		except PortScannerError:
			Log.Error("Failed to create nmap port scanner!")

		self.scanningThread = None
		self.scanningThread = Thread(ThreadStart(self.scanningLoop))
		self.scanningThread.Start()

		ApiManager.RegisterListener("lan", self.apiListener)
		pass

	def OnDisable(self):
		if self.scanningThread is not None:
			self.scanningThread.Abort()

		self.portScanner = None
		pass
	
	def apiListener(self, request):
		d = { "devices": [x.toDict() for x in self.devices] }
		return ApiResponse.Success(Dictionary[str,object](d))
		pass

	def registerCallback(self, call):
		self.onScanDone.append(call)
		pass

	def unregisterCallback(self, call):
		self.onScanDone.remove(call)
		pass


	def scanningLoop(self):
		if self.portScanner is not None:
			try:
				result = self.portScanner.scan(hosts=self.scanHosts, arguments=self.scanArgs)
				Log.Debug("Nmap scan cycle done")
				self.devices = []
				if result is not None:
					for ip, obj in result["scan"].iteritems():
						if "mac" in obj["addresses"]:
							self.devices.append(LanDevice(obj))

				for i in self.onScanDone:
					i()

				Thread.Sleep(self.scanInterval)
				self.scanningLoop()
			except PortScannerError:
				Log.Error("Failed to use nmap port scanner!")
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

	def toDict(self):
		d = {"ip":self.ip, "mac":self.mac, "vendor":self.vendor}
		return d
		pass