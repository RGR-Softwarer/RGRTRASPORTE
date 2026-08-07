import 'package:app_gustavo/app/settings/strings.dart';

class BirthDateValidator {
  static String? validatorBirthDate(String birthDate) {
    if (birthDate.isEmpty) return Strings.errorCompletionRequired;

    final regex = RegExp(r'^\d{2}/\d{2}/\d{4}$');

    if (!regex.hasMatch(birthDate)) {
      return Strings.errorInvalidDate;
    }

    List<String> part = birthDate.split('/');

    int? day = int.tryParse(part[0]);
    int? month = int.tryParse(part[1]);
    int? year = int.tryParse(part[2]);

    DateTime currentDate = DateTime.now();
    DateTime dataNascimentoDT = DateTime(year!, month!, day!);
    int age = currentDate.year - dataNascimentoDT.year;

    if (age < 16 || age >= 120) {
      return Strings.errorInvalidAge;
    }

    if (month < 1 || month > 12) {
      return Strings.errorInvalidDate;
    }

    if (day < 1 || day > DateTime(year, month + 1, 0).day) {
      return Strings.errorInvalidDate;
    }

    return null;
  }
}
