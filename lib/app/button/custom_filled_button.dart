import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:flutter/material.dart';

class CustomFilledButton extends StatelessWidget {
  const CustomFilledButton(
      {required this.label,
      required this.onPressed,
      this.backgroundColor = ColorsTheme.bluePrimary,
      super.key});

  final String label;
  final Color backgroundColor;
  final Function() onPressed;

  @override
  Widget build(BuildContext context) {
    return TextButton(
      onPressed: onPressed,
      style: TextButton.styleFrom(
        padding: const EdgeInsets.symmetric(vertical: 15.0),
        shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.all(Radius.circular(5)),
        ),
        backgroundColor: backgroundColor,
      ),
      child: Text(
        label.toUpperCase(),
        style: const TextStyle(color: ColorsTheme.white),
      ),
    );
  }
}
