using System.ComponentModel;
using System.Reflection;
using System.Text;
using XjjXmm.Infrastructure.DataValidation.Display;
using XjjXmm.Infrastructure.ToolKit;
using IValidatableObject = XjjXmm.Infrastructure.DataValidation.Validators.IValidatableObject;

namespace XjjXmm.Infrastructure.DataValidation.Context
{
	public class FieldInfo
	{
		public PropertyInfo PropertyInfo { get; set; }

		private string? _name;

		public string Name
		{
			get
			{
				if (_name == null)
				{
					_name = PropertyInfo.Name;
				}
				else
				{
					_name = "";
				}
				

				return _name;
			}
		}

		private string? _desc;

		public string Desc
		{
			get
			{
				if (_desc == null)
				{
					var descAttr = PropertyInfo.GetCustomAttribute<DescriptionAttribute>();
					if (descAttr != null)
					{
						_desc = descAttr.Description;
					}
					else
					{
						_desc = Name;
					}
				}

				return _desc;
			}
			set
			{
				_desc = value;
			}
		}

		
		public List<ValidationAttribute> ValidationAttributes { get; set; } = new List<ValidationAttribute>();

		public List<DisplayAttribute> DisplayAttributes { get; set; } = new List<DisplayAttribute>();
			
		public IValidatableObject? ValidatableObjects { get; set; }
		
		public List<FieldInfo> Childrens { get; set; } = new List<FieldInfo>();
		
		public List<FieldInfo> PathInfos { get; set; } = new List<FieldInfo>();


		private string? _path;
		public string Path
		{
			get
			{
				if (_path == null)
				{
					var sb = new StringBuilder();
					foreach(var path in PathInfos)
					{
						sb.Append(path.Name);
						sb.Append(".");
					}

					if (sb.Length > 0)
					{
						sb.Remove(sb.Length - 1, 1);
					}

					_path = sb.ToString();
				}

				return _path;
			}
		}

		private string? _fullPath;
		public string FullPath
		{
			get
			{
				if (_fullPath == null)
				{
					var sb = new StringBuilder();
					foreach(var path in PathInfos)
					{
						sb.Append(path.Name);
						sb.Append(".");
					}

					sb.Append(Name);

					_fullPath = sb.ToString();
				}

				return _fullPath;
			}
		}

	}
}
