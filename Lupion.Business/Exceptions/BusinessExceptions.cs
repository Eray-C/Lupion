namespace Lupion.Business.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }

    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
}

public class RecordNotFoundException : BusinessException
{
    public RecordNotFoundException() : base("KayÄ±t bulunamadÄ±!") { }
}

public class FileNotFoundException : BusinessException
{
    public FileNotFoundException() : base("Dosya bulunamadÄ±!") { }
}

public class PlateAlreadyExistsException : BusinessException
{
    public PlateAlreadyExistsException() : base("Bu plakaya sahip araÃ§ zaten mevcut!") { }
}

public class UserRegistrationException : BusinessException
{
    public UserRegistrationException() : base("KullanÄ±cÄ± kaydÄ± sÄ±rasÄ±nda bir hata oluÅŸtu.") { }
}

public class ConflictException : BusinessException
{
    public ConflictException(string message) : base(message) { }
}

public class LoginInProgressException : BusinessException
{
    public LoginInProgressException() : base("GiriÅŸ iÅŸlemi zaten devam ediyor. LÃ¼tfen bekleyin.") { }
}
