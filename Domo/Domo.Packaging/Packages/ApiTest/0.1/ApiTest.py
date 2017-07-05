from Domo.Modules import *

class Test(TriggerModule):
    @ApiAction
    def Temp(self):
        Log.Info("Temp");
        return "test";

    @ApiAction
    def Temp(self, var):
        Log.Info(var.test);
        return var.test;

class Test2:
    test = "Hello world";