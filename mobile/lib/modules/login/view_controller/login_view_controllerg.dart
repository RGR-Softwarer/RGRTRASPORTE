import 'package:app_gustavo/app/validators/cpf_validator.dart';
import 'package:app_gustavo/repositories/login_network.dart';
import 'package:get/get.dart';

class LoginViewControllerG extends GetxController {
  final LoginNetwork api;

  LoginViewControllerG(this.api);

  final bool statusLogin = false;
  var credential = '';
  var password = '';

  @override
  void onInit() {
    postUserCredentials();
    super.onInit();
  }

  Future<void> postUserCredentials() async {
    var result =
        await api.postLogin(credential.toString(), password.toString());
    result.fold((left) => false, (right) => true);
  }

  bool isCPF(String value) {
    final onlyCharacters = value.replaceAll(r'[-.]', '');
    if (RegExp(r'[^\d]').hasMatch(onlyCharacters) || onlyCharacters.isEmpty) {
      return false;
    }
    return true;
  }

  String validatorEmail(String credential) {
    var validator = Validator.validatorEmail(credential);
    if (validator!.isNotEmpty) {
      return validator;
    }
    return '';
  }

  String validatorCPF(String credential) {
    var validator = Validator.validatorCPF(credential);
    if (validator!.isNotEmpty) {
      return validator;
    }
    return '';
  }
}
