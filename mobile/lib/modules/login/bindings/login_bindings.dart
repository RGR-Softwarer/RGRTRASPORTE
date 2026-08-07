import 'package:app_gustavo/modules/login/view_controller/login_view_controllerg.dart';
import 'package:app_gustavo/repositories/login_network.dart';
import 'package:get/get.dart';

class LoginBind extends Bindings {
  @override
  void dependencies() {
    Get.lazyPut<LoginViewControllerG>(() {
      final LoginNetwork api = LoginNetwork();
      return LoginViewControllerG(api);
    });
  }
}
