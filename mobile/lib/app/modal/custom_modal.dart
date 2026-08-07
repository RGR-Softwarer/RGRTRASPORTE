import 'package:app_gustavo/app/settings/images.dart';
import 'package:app_gustavo/app/text/medium_text.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class CustomModal {
  void modalBottomSheet(context, String label, Widget body) {
    showModalBottomSheet(
        context: context,
        isScrollControlled: true,
        shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.vertical(top: Radius.circular(18.0)),
        ),
        builder: (BuildContext bc) {
          return Wrap(children: [
            Column(
              children: [
                Container(
                  margin:
                      const EdgeInsets.symmetric(horizontal: 20, vertical: 15),
                  child: Row(
                    children: [
                      GestureDetector(
                          onTap: () {
                            Get.back();
                          },
                          child: Image.asset(Images.arrowLeft)),
                      const SizedBox(
                        width: 20,
                      ),
                      MediumText(
                        text: label,
                      ),
                    ],
                  ),
                ),
                body,
              ],
            ),
          ]);
        });
  }
}
