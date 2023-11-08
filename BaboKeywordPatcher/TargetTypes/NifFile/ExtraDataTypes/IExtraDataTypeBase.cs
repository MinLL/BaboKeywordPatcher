using nifly;
using static nifly.niflycpp;

namespace BaboKeywordPatcher.TargetTypes.NifFileTargetType.ExtraDataTypes
{
    public interface IExtraDataTypeBase
    {
        public bool IsValid(NiBlockRefNiExtraData extraDataRef, BlockCache blockCache);
    }
}
