namespace Waes.Diffly.Core.Domain.Entities
{
    /// <summary>
    /// The detail of diff, that tells the offset where diff begins and the length of it.
    /// </summary>
    public class DiffDetail
    {
        public DiffDetail(int offset, int length)
        {
            this.Offset = offset;
            this.Length = length;
        }

        public int Offset { get; set; }
        public int Length { get; set; }


        public override bool Equals(object obj)
        {
            var diffDetail = obj as DiffDetail;
            if (diffDetail == null)
            {
                return false;
            }

            return this.Offset == diffDetail.Offset && this.Length == diffDetail.Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Offset.GetHashCode() * 397) ^ Length.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"Length:{Length}, Offset:{Offset}";
        }
    }
}
