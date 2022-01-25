namespace DAL;

public interface IAccess <T>
{
    public void Create(T _object);
    public List<T> GetAll();
    public T GetById(int id);
    public void Update(T _object);
    public void Delete(T _object);
}