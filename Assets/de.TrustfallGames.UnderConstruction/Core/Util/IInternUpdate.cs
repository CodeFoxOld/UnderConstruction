namespace de.TrustfallGames.UnderConstruction.Core.Util {
    /// <summary>
    /// Interface for intern update objects
    /// </summary>
    public interface IInternUpdate {
        void InternUpdate();

        void RegisterInternUpdate();

        void Init();

        void OnDestroy();
    }
}