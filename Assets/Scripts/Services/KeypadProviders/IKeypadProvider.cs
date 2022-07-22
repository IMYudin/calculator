using System.Collections.Generic;

public interface IKeypadProvider
{
    int KeysInRowCount { get; set; }
    
    IList<KeyModel> GetAllKeys();
    void AddKeys(IList<KeyModel> keys);
}