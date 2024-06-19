namespace GarageDI.Entities;

class MotorCycle : Vehicle
{
    [Beautify("Cylinder volyme")]
    [Include]
    public int CylinderVolyme { get; set; }
}
