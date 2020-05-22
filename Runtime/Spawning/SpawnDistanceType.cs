namespace BrightLib.Pooling.Runtime
{
    public enum SpawnDistanceType 
    {
        Manual,  //Default. Will use the SpawnPoint you pass
        Far,     //Will pick the SpawnPoint closest to the player
        Close,   //Will pick the SpawnPoint fartest to the player
        Random   //Will pick a SpawnPoint randomly
    };
}