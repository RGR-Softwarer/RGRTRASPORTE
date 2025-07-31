using Dominio.Interfaces;

namespace Dominio.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T item);
        string ErrorMessage { get; }
    }

    // Interface para specifications que trabalham com NotificationContext
    public interface INotificationSpecification<in T>
    {
        void ValidateAndNotify(T item, INotificationContext notificationContext);
        bool IsSatisfiedBy(T item);
    }

    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T item);
        public abstract string ErrorMessage { get; }

        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    // Base para specifications com notification
    public abstract class NotificationSpecification<T> : INotificationSpecification<T>
    {
        public abstract bool IsSatisfiedBy(T item);
        public abstract string ErrorMessage { get; }

        public virtual void ValidateAndNotify(T item, INotificationContext notificationContext)
        {
            if (!IsSatisfiedBy(item))
            {
                notificationContext.AddNotification(ErrorMessage);
            }
        }

        public INotificationSpecification<T> And(INotificationSpecification<T> other)
        {
            return new NotificationAndSpecification<T>(this, other);
        }

        public INotificationSpecification<T> Or(INotificationSpecification<T> other)
        {
            return new NotificationOrSpecification<T>(this, other);
        }
    }

    public class AndSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T item)
        {
            return _left.IsSatisfiedBy(item) && _right.IsSatisfiedBy(item);
        }

        public override string ErrorMessage => 
            $"{_left.ErrorMessage} E {_right.ErrorMessage}";
    }

    public class OrSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T item)
        {
            return _left.IsSatisfiedBy(item) || _right.IsSatisfiedBy(item);
        }

        public override string ErrorMessage => 
            $"{_left.ErrorMessage} OU {_right.ErrorMessage}";
    }

    public class NotSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public override bool IsSatisfiedBy(T item)
        {
            return !_specification.IsSatisfiedBy(item);
        }

        public override string ErrorMessage => 
            $"NÃO {_specification.ErrorMessage}";
    }

    // Notification specifications combinators
    public class NotificationAndSpecification<T> : NotificationSpecification<T>
    {
        private readonly INotificationSpecification<T> _left;
        private readonly INotificationSpecification<T> _right;

        public NotificationAndSpecification(INotificationSpecification<T> left, INotificationSpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T item)
        {
            return _left.IsSatisfiedBy(item) && _right.IsSatisfiedBy(item);
        }

        public override string ErrorMessage => 
            $"Ambas condições devem ser atendidas";

        public override void ValidateAndNotify(T item, INotificationContext notificationContext)
        {
            _left.ValidateAndNotify(item, notificationContext);
            _right.ValidateAndNotify(item, notificationContext);
        }
    }

    public class NotificationOrSpecification<T> : NotificationSpecification<T>
    {
        private readonly INotificationSpecification<T> _left;
        private readonly INotificationSpecification<T> _right;

        public NotificationOrSpecification(INotificationSpecification<T> left, INotificationSpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T item)
        {
            return _left.IsSatisfiedBy(item) || _right.IsSatisfiedBy(item);
        }

        public override string ErrorMessage => 
            $"Pelo menos uma condição deve ser atendida";

        public override void ValidateAndNotify(T item, INotificationContext notificationContext)
        {
            var leftSatisfied = _left.IsSatisfiedBy(item);
            var rightSatisfied = _right.IsSatisfiedBy(item);

            if (!leftSatisfied && !rightSatisfied)
            {
                notificationContext.AddNotification(ErrorMessage);
            }
        }
    }
} 
