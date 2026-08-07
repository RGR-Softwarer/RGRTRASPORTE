import 'package:app_gustavo/modules/login/bindings/login_bindings.dart';
import 'package:app_gustavo/modules/login/view/login_view.dart';
import 'package:app_gustavo/modules/register/register_passenger/view/register_passenger_view.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      debugShowCheckedModeBanner: false,
      initialRoute: '/',
      defaultTransition: Transition.native,
      locale: const Locale('pt', 'BR'),
      getPages: [
        GetPage(
          name: "/",
          page: () => const LoginView(),
          binding: LoginBind(),
        ),
        GetPage(
          name: "/login",
          page: () => const LoginView(),
          binding: LoginBind(),
        ),
        GetPage(
          name: "/register_options",
          page: () => const RegisterPassengerView(),
        )
      ],
    );
  }
}
