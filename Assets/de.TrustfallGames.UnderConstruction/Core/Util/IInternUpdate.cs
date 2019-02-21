namespace de.TrustfallGames.UnderConstruction.Core.Util {
    public interface IInternUpdate {
        void InternUpdate();

        void RegisterInternUpdate();

        void Init();

        void OnDestroy();
    }
}