import 'package:app_gustavo/app/settings/colors_theme.dart';
import 'package:app_gustavo/app/settings/strings.dart';
import 'package:flutter/material.dart';

class GenderDropdownButton extends StatefulWidget {
  const GenderDropdownButton({super.key});

  @override
  State<GenderDropdownButton> createState() => _GenderDropdownButtonState();
}

class _GenderDropdownButtonState extends State<GenderDropdownButton> {
  List<DropdownMenuItem<String>> get dropdownItems {
    List<DropdownMenuItem<String>> menuItems = [
      const DropdownMenuItem(value: "Feminino", child: Text("Feminino")),
      const DropdownMenuItem(value: "Masculino", child: Text("Masculino")),
      const DropdownMenuItem(value: "Outros", child: Text("Outros")),
    ];
    return menuItems;
  }

  String selectedValue = "Outros";

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          "Genero",
          style: TextStyle(
            fontWeight: FontWeight.w500,
            fontSize: 16,
            color: ColorsTheme.bluePrimary,
          ),
        ),
        const SizedBox(
          height: 4,
        ),
        DropdownButtonFormField(
          isExpanded: true,
          decoration: const InputDecoration(
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.all(Radius.circular(8.0)),
                borderSide: BorderSide(
                  color: ColorsTheme.bluePrimary,
                ),
              ),
              border: OutlineInputBorder(
                borderRadius: BorderRadius.all(Radius.circular(8.0)),
                borderSide: BorderSide(
                  color: ColorsTheme.bluePrimary,
                ),
              ),
              filled: true,
              fillColor: ColorsTheme.white),
          onChanged: (String? newValue) {
            setState(() {
              selectedValue = newValue!;
            });
          },
          items: dropdownItems,
        ),
      ],
    );
  }
}
