import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:flutter/material.dart';

class SmallText extends StatelessWidget {
  const SmallText({required this.text, super.key});

  final String text;

  @override
  Widget build(BuildContext context) {
    return Text(
      text,
      style: const TextStyle(
        color: ColorsTheme.bluePrimary,
        fontWeight: FontWeight.w400,
        fontSize: 16,
      ),
    );
  }
}
