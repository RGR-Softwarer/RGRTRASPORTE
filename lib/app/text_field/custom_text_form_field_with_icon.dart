import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class CustomTextFormFieldWithIcon extends StatelessWidget {
  const CustomTextFormFieldWithIcon(
      {required this.label,
      required this.suffixIcon,
      required this.focusNode,
      required this.textEditingController,
      this.autocorrect = true,
      this.enableSuggestions = true,
      this.obscureText = false,
      this.autofocus = false,
      this.errorText,
      this.textInputAction = TextInputAction.next,
      this.inputFormatters,
      super.key});

  final bool autocorrect;
  final bool enableSuggestions;
  final String label;
  final bool obscureText;
  final bool autofocus;
  final IconButton suffixIcon;
  final FocusNode focusNode;
  final TextInputAction textInputAction;
  final TextEditingController textEditingController;
  final String? errorText;
  final List<TextInputFormatter>? inputFormatters;

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: const TextStyle(
            fontWeight: FontWeight.w500,
            fontSize: 16,
            color: ColorsTheme.bluePrimary,
          ),
        ),
        const SizedBox(
          height: 4,
        ),
        TextFormField(
          inputFormatters: inputFormatters,
          controller: textEditingController,
          obscureText: obscureText,
          autocorrect: autocorrect,
          autofocus: autofocus,
          textInputAction: textInputAction,
          focusNode: focusNode,
          enableSuggestions: enableSuggestions,
          decoration: InputDecoration(
              suffixIcon: suffixIcon,
              errorStyle: const TextStyle(
                fontSize: 16,
              ),
              errorText: errorText,
              border: customBorder(),
              errorBorder: const OutlineInputBorder(
                borderRadius: BorderRadius.all(Radius.circular(8.0)),
                borderSide: BorderSide(
                  color: Colors.red,
                ),
              ),
              enabledBorder: customBorder(),
              focusedBorder: customBorder(),
              fillColor: ColorsTheme.bluePrimary),
        ),
      ],
    );
  }

  OutlineInputBorder customBorder() => const OutlineInputBorder(
        borderRadius: BorderRadius.all(Radius.circular(8.0)),
        borderSide: BorderSide(
          color: ColorsTheme.bluePrimary,
        ),
      );
}
