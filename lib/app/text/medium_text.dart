import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:flutter/material.dart';

class MediumText extends StatelessWidget {
  const MediumText({required this.text, super.key});

  final String text;

  @override
  Widget build(BuildContext context) {
    return Text(
      text,
      style: const TextStyle(
        color: ColorsTheme.bluePrimary,
        fontWeight: FontWeight.bold,
        fontSize: 20,
      ),
    );
  }
}
