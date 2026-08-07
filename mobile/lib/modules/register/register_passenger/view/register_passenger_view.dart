import 'package:app_gustavo/app/button/custom_filled_button.dart';
import 'package:app_gustavo/app/button/gender_dropdown_button.dart';
import 'package:app_gustavo/app/mask/mask.dart';
import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:app_gustavo/app/settings/images.dart';
import 'package:app_gustavo/app/settings/strings.dart';
import 'package:app_gustavo/app/text/big_text.dart';
import 'package:app_gustavo/app/text/small_text.dart';
import 'package:app_gustavo/app/text_field/custom_text_form_field.dart';
import 'package:app_gustavo/app/text_field/custom_text_form_field_with_icon.dart';
import 'package:app_gustavo/app/validators/birth_date_validator.dart';
import 'package:app_gustavo/app/validators/cpf_validator.dart';
import 'package:app_gustavo/app/validators/string_input_validator.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';

class RegisterPassengerView extends StatefulWidget {
  const RegisterPassengerView({super.key});

  @override
  State<RegisterPassengerView> createState() => _RegisterPassengerViewState();
}

class _RegisterPassengerViewState extends State<RegisterPassengerView> {
  late FocusNode fullNameFocusNode;
  late FocusNode birthDateFocusNode;
  late FocusNode cpfFocusNode;
  late FocusNode emailFocusNode;
  late FocusNode passwordFocusNode;
  late FocusNode confirmPasswordFocusNode;
  late FocusNode phoneFocusNode;
  late final TextEditingController fullNameEditingController;
  late final TextEditingController birthDateEditingController;
  late final TextEditingController cpfEditingController;
  late final TextEditingController emailEditingController;
  late final TextEditingController passwordEditingController;
  late final TextEditingController confirmPasswordEditingController;
  late final TextEditingController phoneEditingController;

  bool isPasswordVisible = false;
  bool isConfirmPasswordVisible = false;
  bool isCheckedTerms = false;

  String fullNameError = "";
  String birthDateError = "";
  String cpfError = "";
  String emailError = "";
  String passwordError = "";
  String phoneError = "";

  @override
  void initState() {
    fullNameFocusNode = FocusNode();
    birthDateFocusNode = FocusNode();
    cpfFocusNode = FocusNode();
    emailFocusNode = FocusNode();
    passwordFocusNode = FocusNode();
    confirmPasswordFocusNode = FocusNode();
    phoneFocusNode = FocusNode();

    fullNameEditingController = TextEditingController();
    birthDateEditingController = TextEditingController();
    birthDateEditingController.text = "";
    cpfEditingController = TextEditingController();
    emailEditingController = TextEditingController();
    passwordEditingController = TextEditingController();
    confirmPasswordEditingController = TextEditingController();
    phoneEditingController = TextEditingController();
    super.initState();
  }

  @override
  void dispose() {
    fullNameEditingController.dispose();
    birthDateEditingController.dispose();
    cpfEditingController.dispose();
    emailEditingController.dispose();
    passwordEditingController.dispose();
    confirmPasswordEditingController.dispose();
    phoneEditingController.dispose();

    fullNameFocusNode.dispose();
    birthDateFocusNode.dispose();
    cpfFocusNode.dispose();
    emailFocusNode.dispose();
    passwordFocusNode.dispose();
    confirmPasswordFocusNode.dispose();
    phoneFocusNode.dispose();
    super.dispose();
  }

  Color getColor(Set<MaterialState> states) {
    const Set<MaterialState> interactiveStates = <MaterialState>{
      MaterialState.selected,
    };
    if (states.any(interactiveStates.contains)) {
      return ColorsTheme.bluePrimary;
    }
    return ColorsTheme.white;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Container(
          margin:
              const EdgeInsets.only(left: 24, top: 50, right: 24, bottom: 30),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.start,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              GestureDetector(
                  onTap: () {
                    Get.back();
                    Get.back();
                  },
                  child: Image.asset(Images.arrowLeft)),
              const SizedBox(height: 10),
              const BigText(text: Strings.createYourAccount),
              const SmallText(text: Strings.fillInTheFollowingInformation),
              const SizedBox(
                height: 30,
              ),
              CustomTextFormField(
                  label: Strings.fullName,
                  focusNode: fullNameFocusNode,
                  textEditingController: fullNameEditingController),
              const SizedBox(
                height: 30,
              ),
              CustomTextFormFieldWithIcon(
                label: Strings.birthDate,
                inputFormatters: [CustomMask.birthDateMask],
                suffixIcon: IconButton(
                  color: ColorsTheme.bluePrimary,
                  icon: const Icon(
                    Icons.calendar_today_outlined,
                  ),
                  onPressed: () async {
                    DateTime? pickedDate = await showDatePicker(
                        context: context,
                        initialDate: DateTime.now(),
                        firstDate: DateTime(2000),
                        lastDate: DateTime(2101));
                    if (pickedDate != null) {
                      String formattedDate =
                          DateFormat('dd/MM/yyyy').format(pickedDate);
                      setState(() {
                        birthDateEditingController.text = formattedDate;
                      });
                    }
                  },
                ),
                focusNode: birthDateFocusNode,
                textEditingController: birthDateEditingController,
              ),
              const SizedBox(
                height: 30,
              ),
              CustomTextFormField(
                label: Strings.cpf,
                focusNode: cpfFocusNode,
                textEditingController: cpfEditingController,
                inputFormatters: [CustomMask.cpfMask],
              ),
              const SizedBox(
                height: 30,
              ),
              CustomTextFormField(
                label: Strings.email,
                focusNode: emailFocusNode,
                textEditingController: emailEditingController,
              ),
              const SizedBox(
                height: 30,
              ),
              CustomTextFormFieldWithIcon(
                label: Strings.password,
                textEditingController: passwordEditingController,
                autocorrect: false,
                enableSuggestions: false,
                focusNode: passwordFocusNode,
                textInputAction: TextInputAction.done,
                obscureText: !isPasswordVisible,
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
              const SizedBox(
                height: 30,
              ),
              CustomTextFormFieldWithIcon(
                label: Strings.confirmPassword,
                textEditingController: confirmPasswordEditingController,
                autocorrect: false,
                enableSuggestions: false,
                focusNode: confirmPasswordFocusNode,
                textInputAction: TextInputAction.done,
                obscureText: !isConfirmPasswordVisible,
                suffixIcon: IconButton(
                  color: ColorsTheme.bluePrimary,
                  icon: Icon(
                    isConfirmPasswordVisible
                        ? Icons.visibility
                        : Icons.visibility_off,
                  ),
                  onPressed: () {
                    setState(() {
                      isConfirmPasswordVisible = !isConfirmPasswordVisible;
                    });
                  },
                ),
              ),
              const SizedBox(
                height: 30,
              ),
              CustomTextFormField(
                label: Strings.phone,
                focusNode: phoneFocusNode,
                textEditingController: phoneEditingController,
                inputFormatters: [CustomMask.phoneMask],
              ),
              const SizedBox(
                height: 30,
              ),
              const GenderDropdownButton(),
              const SizedBox(
                height: 30,
              ),
              Row(
                children: [
                  Checkbox(
                    checkColor: ColorsTheme.white,
                    fillColor: MaterialStateProperty.resolveWith(getColor),
                    value: isCheckedTerms,
                    onChanged: (bool? value) {
                      setState(() {
                        isCheckedTerms = value!;
                      });
                    },
                  ),
                  Flexible(
                    child: GestureDetector(
                      onTap: () {
                        // TODO(): COLOCAR AQUI ROTA PARA OS TERMOS DE USO - DEFINIR COMO VAI SER
                      },
                      child: const Text(
                        Strings.acceptTerms,
                        maxLines: 2,
                        style: TextStyle(
                          color: ColorsTheme.bluePrimary,
                          fontWeight: FontWeight.w400,
                          fontSize: 16,
                        ),
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(
                height: 30,
              ),
              SizedBox(
                width: double.infinity,
                child: CustomFilledButton(
                  label: Strings.register,
                  onPressed: () {
                    validatorAll();
                    var gg = BirthDateValidator.validatorBirthDate(
                        birthDateEditingController.text);
                    print(gg);
                  },
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void validatorAll() {
    clean();
    fullNameError = StringInputValidator.validatorMinCharacters(
        fullNameEditingController.text)!;
    birthDateError =
        BirthDateValidator.validatorBirthDate(birthDateEditingController.text)!;
    cpfError = Validator.validatorCPF(cpfEditingController.text)!;
    emailError = Validator.validatorEmail(emailEditingController.text)!;
  }

  void clean() {
    String fullNameError = "";
    String birthDateError = "";
    String cpfError = "";
    String emailError = "";
    String passwordError = "";
    String phoneError = "";
  }
}
