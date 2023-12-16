import 'package:app_gustavo/app/button/custom_filled_button.dart';
import 'package:app_gustavo/app/modal/custom_modal.dart';
import 'package:app_gustavo/app/text_field/custom_text_form_field.dart';
import 'package:app_gustavo/app/text_field/custom_text_form_field_with_icon.dart';
import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:app_gustavo/app/settings/images.dart';
import 'package:app_gustavo/app/settings/strings.dart';
import 'package:app_gustavo/modules/login/view_controller/login_view_controllerg.dart';
import 'package:app_gustavo/modules/register/register_passenger/view/register_passenger_view.dart';
import 'package:either_dart/either.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class LoginView extends StatefulWidget {
  const LoginView({super.key});

  @override
  State<LoginView> createState() => _LoginViewState();
}

class _LoginViewState extends State<LoginView> {
  bool isPasswordVisible = false;
  final LoginViewControllerG loginViewControllerG = Get.find();
  late FocusNode credentialFocusNode;
  late FocusNode passwordFocusNode;
  late final TextEditingController credentialEditingController;
  late final TextEditingController passwordEditingController;
  String credentialError = '';
  String passwordError = '';

  @override
  void initState() {
    credentialEditingController = TextEditingController();
    passwordEditingController = TextEditingController();
    credentialFocusNode = FocusNode();
    passwordFocusNode = FocusNode();
    super.initState();
  }

  @override
  void dispose() {
    credentialFocusNode.dispose();
    passwordFocusNode.dispose();
    credentialEditingController.dispose();
    passwordEditingController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    return Scaffold(
      backgroundColor: ColorsTheme.white,
      body: SingleChildScrollView(
        child: Container(
          margin: const EdgeInsets.symmetric(horizontal: 24),
          child: Column(
            children: [
              Container(
                alignment: Alignment.center,
                margin: EdgeInsets.symmetric(
                    horizontal: 60, vertical: size.height * .08),
                child: Image.asset(Images.logo),
              ),
              CustomTextFormField(
                textEditingController: credentialEditingController,
                label: Strings.emailOrCPF,
                focusNode: credentialFocusNode,
                errorText: credentialError.isEmpty ? null : credentialError,
              ),
              const SizedBox(
                height: 10,
              ),
              CustomTextFormFieldWithIcon(
                label: Strings.password,
                textEditingController: passwordEditingController,
                autocorrect: false,
                enableSuggestions: false,
                focusNode: passwordFocusNode,
                textInputAction: TextInputAction.done,
                obscureText: !isPasswordVisible,
                errorText: passwordError.isEmpty ? null : passwordError,
                suffixIcon: IconButton(
                  color: ColorsTheme.bluePrimary,
                  icon: Icon(
                    isPasswordVisible ? Icons.visibility : Icons.visibility_off,
                  ),
                  onPressed: () {
                    setState(() {
                      isPasswordVisible = !isPasswordVisible;
                    });
                  },
                ),
              ),
              SizedBox(
                height: size.height * .2,
              ),
              SizedBox(
                width: double.infinity,
                child: CustomFilledButton(
                  label: Strings.enter,
                  onPressed: () {
                    passwordError = '';
                    credentialError = '';
                    if (credentialEditingController.text.isEmpty) {
                      setState(() {
                        credentialError = Strings.errorCompletionRequired;
                      });
                    } else {
                      if (loginViewControllerG
                          .isCPF(credentialEditingController.text)) {
                        setState(() {
                          credentialError = loginViewControllerG
                              .validatorCPF(credentialEditingController.text);
                        });
                      } else {
                        setState(() {
                          credentialError = loginViewControllerG
                              .validatorEmail(credentialEditingController.text);
                        });
                      }
                    }
                    if (passwordEditingController.text.isEmpty) {
                      setState(() {
                        passwordError = Strings.errorCompletionRequired;
                      });
                    }
                    if (passwordError.isEmpty && credentialError.isEmpty) {
                      loginViewControllerG.credential =
                          credentialEditingController.text;
                      loginViewControllerG.password =
                          passwordEditingController.text;
                      var result = loginViewControllerG.postUserCredentials();
                      if (result is Right) {
                      } else {
                        const snackBar = SnackBar(
                          content: Text(Strings.customerNotFound),
                        );
                        ScaffoldMessenger.of(context).showSnackBar(snackBar);
                      }
                    }
                  },
                ),
              ),
              const SizedBox(
                height: 15,
              ),
              SizedBox(
                width: double.infinity,
                child: CustomFilledButton(
                  label: Strings.registerUser,
                  onPressed: () {
                    CustomModal().modalBottomSheet(
                        context,
                        Strings.iam,
                        Container(
                          margin: const EdgeInsets.symmetric(
                              horizontal: 24, vertical: 24),
                          child: Column(
                            children: [
                              SizedBox(
                                width: double.infinity,
                                child: CustomFilledButton(
                                  label: Strings.agency,
                                  onPressed: () {},
                                ),
                              ),
                              const SizedBox(
                                height: 20,
                              ),
                              SizedBox(
                                width: double.infinity,
                                child: CustomFilledButton(
                                  label: Strings.passenger,
                                  onPressed: () {
                                    Get.to(const RegisterPassengerView());
                                  },
                                ),
                              ),
                            ],
                          ),
                        ));
                  },
                ),
              ),
              const SizedBox(
                height: 15,
              ),
              Center(
                  child: GestureDetector(
                onTap: () {},
                child: const Text(
                  Strings.forgotMyPassword,
                  style: TextStyle(
                    color: ColorsTheme.bluePrimary,
                    fontWeight: FontWeight.w600,
                    decoration: TextDecoration.underline,
                  ),
                ),
              )),
              const SizedBox(
                height: 24,
              ),
            ],
          ),
        ),
      ),
    );
  }
}
