namespace KnitterNotebook.Validators
{
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates data of object
        /// </summary>
        /// <param name="value">Object whose data needs to be validated</param>
        /// <returns>True if objects's data is correct, otherwise false</returns>
        bool Validate(T value);
    }
}