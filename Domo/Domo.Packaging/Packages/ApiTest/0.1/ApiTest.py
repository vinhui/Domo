from Domo.Modules import *

@ApiAction
def Play():
    Log.Info("Play");

class Test(TriggerModule):
    @ApiAction
    def Temp(self):
        Log.Info("Temp");