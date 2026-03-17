namespace Lupion.Business.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }

    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
}

public class RecordNotFoundException : BusinessException
{
    public RecordNotFoundException() : base("Kayıt bulunamadı!") { }
}

public class FileNotFoundException : BusinessException
{
    public FileNotFoundException() : base("Dosya bulunamadı!") { }
}

public class PlateAlreadyExistsException : BusinessException
{
    public PlateAlreadyExistsException() : base("Bu plakaya sahip araç zaten mevcut!") { }
}

public class UserRegistrationException : BusinessException
{
    public UserRegistrationException() : base("Kullanıcı kaydı sırasında bir hata oluştu.") { }
}

public class ConflictException : BusinessException
{
    public ConflictException(string message) : base(message) { }
}

public class LoginInProgressException : BusinessException
{
    public LoginInProgressException() : base("Giriş işlemi zaten devam ediyor. Lütfen bekleyin.") { }
}
