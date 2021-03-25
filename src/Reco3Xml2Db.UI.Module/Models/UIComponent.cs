using Reco3Xml2Db.Library;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;

namespace Reco3Xml2Db.UI.Module.Models {
	public class UIComponent : ModelBase { // INotifyPropertyChanged {
		//private bool _isChecked;
		//public bool IsChecked {
		//	get => _isChecked;
		//	set {
		//		if (value == _isChecked) return;
		//		_isChecked = value;
		//		OnPropertyChanged();
		//	}
		//}

		public ComponentInfo Component { get; set; }

		//public event PropertyChangedEventHandler PropertyChanged;

		//protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
		//	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		//}
	}
}
