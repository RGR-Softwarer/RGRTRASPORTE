import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:flutter/material.dart';

class BigText extends StatelessWidget {
  const BigText({required this.text, super.key});

  final String text;

  @override
  Widget build(BuildContext context) {
    return Text(
      text,
      style: const TextStyle(
        color: ColorsTheme.bluePrimary,
        fontWeight: FontWeight.bold,
        fontSize: 26,
      ),
    );
  }
}
