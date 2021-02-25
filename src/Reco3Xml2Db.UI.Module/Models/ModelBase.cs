using Prism.Mvvm;

namespace Reco3Xml2Db.UI.Module.Models {
	public class ModelBase : BindableBase {
		private bool _isChecked;
		public bool IsChecked {
			get => _isChecked;
			set {
				SetProperty(ref _isChecked, value);
			}
		}
	}
}
