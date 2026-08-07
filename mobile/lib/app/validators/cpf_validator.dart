import 'package:app_gustavo/app/settings/strings.dart';

class Validator {
  static String? validatorCPF(String cpf) {
    const String errorCPF = Strings.errorInvalidLogin;
    if (cpf.isEmpty) return Strings.errorCompletionRequired;
    var numbersCpf = cpf.replaceAll(RegExp(r'[^0-9]'), '');
    if (numbersCpf.length != 11) return errorCPF;
    if (RegExp(r'^(\d)\1*$').hasMatch(numbersCpf)) return errorCPF;
    var digits = numbersCpf.split('').map(int.parse).toList();
    var calcDv1 = 0;
    for (final i in Iterable<int>.generate(9, (i) => 10 - i)) {
      calcDv1 += digits[10 - i] * i;
    }
    calcDv1 %= 11;
    var dv1 = calcDv1 < 2 ? 0 : 11 - calcDv1;
    if (digits[9] != dv1) return errorCPF;
    var calcDv2 = 0;
    for (final i in Iterable<int>.generate(10, (i) => 11 - i)) {
      calcDv2 += digits[11 - i] * i;
    }
    calcDv2 %= 11;
    var dv2 = calcDv2 < 2 ? 0 : 11 - calcDv2;
    if (digits[10] != dv2) return errorCPF;
    return '';
  }

  static String? validatorEmail(String email) {
    if (RegExp(
      r"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ).hasMatch(email)) {
      return "";
    }
    return Strings.errorInvalidLogin;
  }
}
