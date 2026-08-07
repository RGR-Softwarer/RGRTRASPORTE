import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class CustomTextFormField extends StatelessWidget {
  const CustomTextFormField(
      {required this.label,
      required this.focusNode,
      required this.textEditingController,
      this.autocorrect = true,
      this.enableSuggestions = true,
      this.obscureText = false,
      this.autofocus = false,
      this.errorText,
      this.inputFormatters,
      this.textInputAction = TextInputAction.next,
      super.key});

  final bool autocorrect;
  final bool enableSuggestions;
  final String label;
  final String? errorText;
  final bool obscureText;
  final bool autofocus;
  final TextInputAction textInputAction;
  final FocusNode focusNode;
  final TextEditingController textEditingController;
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
          controller: textEditingController,
          focusNode: focusNode,
          textInputAction: textInputAction,
          inputFormatters: inputFormatters,
          obscureText: obscureText,
          autocorrect: autocorrect,
          autofocus: autofocus,
          enableSuggestions: enableSuggestions,
          decoration: InputDecoration(
              border: customBorder(),
              errorText: errorText,
              errorStyle: const TextStyle(
                fontSize: 16,
              ),
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
