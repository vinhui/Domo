from Domo.Modules import *

from System.Net import WebClient, WebException, HttpStatusCode

import json
import random
import string

class SpotifyInterface(HardwareInterfaceModule):
	webClient = WebClient()
	webClient.Headers.Add("Origin", "https://open.spotify.com")

	oauthTokenURL = "http://open.spotify.com/token"
	csrfToken = None
	oauthToken = None

	spotifyCsrfTokenPath = "/simplecsrf/token.json?cors=&ref="
	spotifyPort = 4371

	spotifyPortSearchStart = 4370
	#spotifyPortSearchStart = 4380
	spotifyPortSearchEnd = 4389

	subdomain = None
	@property
	def spotifyHostUrl(self):
		return self.spotifyHostUrlNoPort + ":" + str(self.spotifyPort)
		pass

	@property
	def spotifyHostUrlNoPort(self):
		if self.subdomain is None:
			self.subdomain = ''.join(random.choice(string.ascii_lowercase + string.digits) for _ in range(6))
		return "https://{0}.spotilocal.com".format(self.subdomain)
		pass

	def OnEnable(self):
		Log.Info("Initializing spotify interface")
		#self.spotifyPort = self.findPort(self.spotifyPortSearchStart,
		#self.spotifyPortSearchEnd)
		self.getOauthToken()
		self.getCsrfToken()
		Log.Info("Spotify interface is initialized")
		self.isInitialized = True
		pass

	def OnDisable(self):
		pass

	def getOauthToken(self):
		response = self.webClient.DownloadString(self.oauthTokenURL)
		j = json.loads(response)
		self.oauthToken = j["t"]

		Log.Debug("Spotify oauth token: {0}".format(self.oauthToken))
		pass

	def getCsrfToken(self):
		url = self.spotifyHostUrl + self.spotifyCsrfTokenPath
		response = self.webClient.DownloadString(url)
		j = json.loads(response)
		self.csrfToken = j["token"]

		Log.Debug("Spotify csrf token: {0}".format(self.csrfToken))
		pass

	def findPort(self, start, end):
		for i in range(start, end + 1):
			try:
				response = self.webClient.DownloadString(self.spotifyHostUrlNoPort + ":" + str(i))
				Log.Debug("Local spotify server is running on port {0}".format(i))
				return i
			except WebException as e:
				if e.Response is not None and e.Response.StatusCode.value__ == HttpStatusCode.NotFound.value__:
					Log.Debug("Local spotify server is running on port {0}".format(i))
					return i
				else:
					Log.Debug("Spotify server is NOT running on port {0}".format(i))
			pass

		Log.Warning("Failed to get the port on which the local spotify server is running!")
		pass

	def get(self, path):
		passownloadString(self.spotifyHostUrl + path + "?csrf={0}&oauth={1}".format(self.csrfToken, self.oauthToken))
		j = json.loads(response)

		return j
		pass