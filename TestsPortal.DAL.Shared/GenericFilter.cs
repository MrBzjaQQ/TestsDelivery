using System.Linq.Expressions;

namespace TestsDelivery.DAL.Shared
{
    public class GenericFilter<TObject>
    {
        public GenericFilter(params Expression<Func<TObject, bool>>[] specifications)
        {
            if (specifications == null)
                throw new ArgumentNullException(nameof(specifications));

            foreach (var spec in specifications)
                AddAndSpecification(new Specification<TObject>(spec));
        }

        public ISpecification<TObject> Specification { get; set; }

        public ISorting<TObject> Sorting { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }

        public void AddAndSpecification(Specification<TObject> newSpecification)
        {
            Specification = Specification == null ? newSpecification : Specification.And(newSpecification);
        }

        public void AddOrSpecification(Specification<TObject> newSpecification)
        {
            Specification = Specification == null ? newSpecification : Specification.Or(newSpecification);
        }
    }
}