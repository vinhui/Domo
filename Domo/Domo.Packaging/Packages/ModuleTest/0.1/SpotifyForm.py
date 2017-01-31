from Domo.Modules import *

from System.Drawing import Point, Color, Size, Brush, SolidBrush 
from System.Threading import Thread, ThreadStart
from System.Windows.Forms import (
	Application, 
	Form, 
	DialogResult,
	MethodInvoker,
	Timer,
	Label, 
	Button, 
	NumericUpDown, 
	ColorDialog,
	TrackBar,
	)

class SpotifyForm(TriggerModule):
	form = None

	def __init__(self):
		pass

	def OnEnable(self):
		TriggerModule.init(self, SpotifySensor, SpotifyController)
		self.formThread = None
		self.formThread = Thread(ThreadStart(self.CreateForm))
		self.formThread.Start()
		pass
	
	def OnDisable(self):
		if self.form is not None and self.form.Visible:
			self.form.Invoke(MethodInvoker(self.form.Close))

		if self.formThread is not None:
			self.formThread.Abort()

		self.form = None
		pass

	def CreateForm(self):
		self.form = SpotifyTestForm(self.sensor, self.controller)
		Application.Run(self.form)
		pass

class SpotifyTestForm(Form):
	def __init__(self, sensor, controller):
		self.Text = "Spotify Controls"
		self.Name = "Test"
		self.sensor = sensor
		self.controller = controller

		self.createControls()
		self.startTimerLoop()
		pass

	def createControls(self):
		l = Label()
		l.Text = "Is Playing:"
		l.Location = Point(10, 10)
		self.Controls.Add(l)

		self.playingLabel = Label()
		self.playingLabel.Text = "false"
		self.playingLabel.Location = Point(110, 10)
		self.Controls.Add(self.playingLabel)

		l = Label()
		l.Text = "Track:"
		l.Location = Point(10, 35)
		self.Controls.Add(l)

		self.trackLabel = Label()
		self.trackLabel.Text = ""
		self.trackLabel.Location = Point(110, 35)
		self.trackLabel.Size = Size(150, 25)
		self.Controls.Add(self.trackLabel)

		l = Label()
		l.Text = "Artist:"
		l.Location = Point(10, 60)
		self.Controls.Add(l)

		self.artistLabel = Label()
		self.artistLabel.Text = ""
		self.artistLabel.Location = Point(110, 60)
		self.artistLabel.Size = Size(150, 25)
		self.Controls.Add(self.artistLabel)

		b = Button()
		b.Text = "Play"
		b.Location = Point(10, 85)
		b.Click += self.playClicked
		self.Controls.Add(b)

		b = Button()
		b.Text = "Pause"
		b.Location = Point(110, 85)
		b.Click += self.pauseClicked
		self.Controls.Add(b)
		pass

	def startTimerLoop(self):
		timer = Timer();
		timer.Interval = 2000;
		timer.Tick += self.timerTick;
		timer.Start();
		pass

	def timerTick(self, *args):
		self.updateStatus()
		pass

	def playClicked(self, *args):
		self.controller.play()
		self.updateStatus()
		pass

	def pauseClicked(self, *args):
		self.controller.pause()
		self.updateStatus()
		pass

	def updateStatus(self):
		self.playingLabel.Text = str(self.sensor.isPlaying).lower()
		self.trackLabel.Text = self.sensor.track
		self.artistLabel.Text = self.sensor.artist
		pass