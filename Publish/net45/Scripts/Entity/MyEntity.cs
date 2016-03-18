using Pi.Framework;
using socket4net;
using System.Collections.Generic;
namespace Pi.Gen
{
    /// <summary>
    ///     实体定义
    /// </summary>
    public class MyEntity : Entity
    {
        protected override void OnInit(ObjArg arg)
        {
            base.OnInit(arg);

			// inject blocks
            Inject(new []
            {
				BlockMaker.Create(EPid.One),
				BlockMaker.Create(EPid.Two),
				BlockMaker.Create(EPid.Three),

            });
        }
      
        protected override void SpawnComponents()
        {
            base.SpawnComponents();

			// add components
			AddComponent<Shared.SampleComponentBase>();

        }

		#region properties
      public int One
        {
            get { return Get<int>((short)EPid.One); }
            set { Set<int>((short)EPid.One, value); }
        }
      public int Two
        {
            get { return Get<int>((short)EPid.Two); }
            set { IncTo<int>((short)EPid.Two, value); }
        }
      public List<string> Three
        {
            get { return GetList<string>((short)EPid.Three); }
            set { RemoveAll((short)EPid.Three); AddRange<string>((short)EPid.Three, value); }
        }

		#endregion
    }
}

	