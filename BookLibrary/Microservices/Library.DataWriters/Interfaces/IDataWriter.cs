namespace Library.DataWriters.Interfaces
{
    public interface IDataWriter<TDto>
    {
        void Add(TDto dto);
        void Remove(string id);
        void Update(TDto dto);
    }
}
