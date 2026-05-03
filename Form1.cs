using System;
using System.Linq;
using System.Windows.Forms;

namespace Calculator {
  public partial class Form1 : Form {
    public Form1() {
      InitializeComponent();
    }

    enum MathOperator {
      PLUS = '+',
      MINUS = '-',
      MULTIPLICATION = '×',
      DIVISION = '÷'
    }

    private void ConcatToResultLabel(string newText) {
      if (LblResult.Text.Length < 15) {
        LblResult.Text += newText;
      }
    }

    private void Btn0_Click(object sender, EventArgs e) {
      ConcatToResultLabel("0");
    }

    private void Btn00_Click(object sender, EventArgs e) {
      ConcatToResultLabel("00");
    }

    private void BtnDot_Click(object sender, EventArgs e) {
      if (string.IsNullOrEmpty(LblResult.Text)) {
        LblResult.Text = "0.";
      } else if (IsOperator(LblResult.Text.Last())) {
        ConcatToResultLabel("0.");
      } else if (!DotIsExist()) {
        ConcatToResultLabel(".");
      }
    }

    private void BtnEqual_Click(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(LblResult.Text)) {
        LblPreviousResult.Text = LblResult.Text + " =";
        LblResult.Text = Calculate();
      }
    }

    private void Btn1_Click(object sender, EventArgs e) {
      ConcatToResultLabel("1");
    }

    private void Btn2_Click(object sender, EventArgs e) {
      ConcatToResultLabel("2");
    }

    private void Btn3_Click(object sender, EventArgs e) {
      ConcatToResultLabel("3");
    }

    private void BtnPlus_Click(object sender, EventArgs e) {
      if (string.IsNullOrEmpty(LblResult.Text))
        return;

      if (!ResultIsEndWithOperation()) {
        if (LblResult.Text.Last() == '.') {
          LblResult.Text = LblResult.Text.Remove(LblResult.Text.Length - 1);
        }

        ConcatToResultLabel(((char)MathOperator.PLUS).ToString());
      }
        
    }

    private void Btn4_Click(object sender, EventArgs e) {
      ConcatToResultLabel("4");
    }

    private void Btn5_Click(object sender, EventArgs e) {
      ConcatToResultLabel("5");
    }

    private void Btn6_Click(object sender, EventArgs e) {
      ConcatToResultLabel("6");
    }

    private void BtnMinus_Click(object sender, EventArgs e) {
      if (string.IsNullOrEmpty(LblResult.Text))
        return;

      if (!ResultIsEndWithOperation())
        ConcatToResultLabel(((char)MathOperator.MINUS).ToString());
    }

    private void Btn7_Click(object sender, EventArgs e) {
      ConcatToResultLabel("7");
    }

    private void Btn8_Click(object sender, EventArgs e) {
      ConcatToResultLabel("8");
    }

    private void Btn9_Click(object sender, EventArgs e) {
      ConcatToResultLabel("9");
    }

    private void BtnMultiplication_Click(object sender, EventArgs e) {
      if (string.IsNullOrEmpty(LblResult.Text))
        return;

      if (!ResultIsEndWithOperation())
        ConcatToResultLabel(((char)MathOperator.MULTIPLICATION).ToString());
    }

    private void BtnClear_Click(object sender, EventArgs e) {
      LblResult.Text = string.Empty;
      LblPreviousResult.Text = string.Empty;
    }

    private void BtnBackspace_Click(object sender, EventArgs e) {
      if (!string.IsNullOrEmpty(LblResult.Text)) {
        LblResult.Text = LblResult.Text.Remove(LblResult.Text.Length - 1);
      }
    }

    private void BtnDivision_Click(object sender, EventArgs e) {
      if (string.IsNullOrEmpty(LblResult.Text))
        return;

      if (!ResultIsEndWithOperation())
        ConcatToResultLabel(((char)MathOperator.DIVISION).ToString());
    }

    private bool ResultIsEndWithOperation() {
      return LblResult.Text.Last() == (char)MathOperator.PLUS || LblResult.Text.Last() == (char)MathOperator.MINUS ||
          LblResult.Text.Last() == (char)MathOperator.MULTIPLICATION || LblResult.Text.Last() == (char)MathOperator.DIVISION;
    }

    private int IndexPrevOper(string text, int currentOperIndex) {
      for (int i = currentOperIndex - 1; i >= 0; --i) {
        if (text[i] == (char)MathOperator.MULTIPLICATION || text[i] == (char)MathOperator.DIVISION ||
            text[i] == (char)MathOperator.PLUS || text[i] == (char)MathOperator.MINUS) {

          return i;
        }
      }

      return -1;
    }

    private int IndexNextOper(string text, int currentOperIndex) {
      for (int i = currentOperIndex + 1; i < text.Length; ++i) {
        if (text[i] == (char)MathOperator.MULTIPLICATION || text[i] == (char)MathOperator.DIVISION ||
            text[i] == (char)MathOperator.PLUS || text[i] == (char)MathOperator.MINUS) {

          return i;
        }
      }

      return text.Length;
    }

    private string PerformOpertion(string text, int operIndex, MathOperator opertionType) {
      int indexPrevOper = IndexPrevOper(text, operIndex);
      int indexNextOper = IndexNextOper(text, operIndex);
      double numBeforeOper = Convert.ToDouble(text.Substring(indexPrevOper + 1, operIndex - indexPrevOper - 1));
      double numAfterOper = Convert.ToDouble(text.Substring(operIndex + 1, indexNextOper - (operIndex + 1)));
      text = text.Remove(indexPrevOper + 1, indexNextOper - (indexPrevOper + 1));

      switch (opertionType) {
        case MathOperator.MULTIPLICATION:
          text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper * numAfterOper));
          break;
        case MathOperator.DIVISION:
          text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper / numAfterOper));
          break;
        case MathOperator.PLUS:
          text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper + numAfterOper));
          break;
        case MathOperator.MINUS:
          text = text.Insert(indexPrevOper + 1, Convert.ToString(numBeforeOper - numAfterOper));
          break;
      }

      return text;
    }

    private string Calculate() {
      string text = LblResult.Text;
      while (!double.TryParse(text, out _)) {
        for (int i = 0; i < text.Length; ++i) {
          if (text[i] == (char)MathOperator.MULTIPLICATION) {
            text = PerformOpertion(text, i, MathOperator.MULTIPLICATION);
          } else if (text[i] == (char)MathOperator.DIVISION) {
            text = PerformOpertion(text, i, MathOperator.DIVISION);
          }
        }

        for (int i = 0; i < text.Length; ++i) {
          if (text[i] == (char)MathOperator.PLUS) {
            text = PerformOpertion(text, i, MathOperator.PLUS);
          } else if (text[i] == (char)MathOperator.MINUS) {
            text = PerformOpertion(text, i, MathOperator.MINUS);
          }
        }
      }

      return text;
    }

    private bool IsOperator(char c) {
      return c == (char)MathOperator.MULTIPLICATION || c == (char)MathOperator.DIVISION ||
          c == (char)MathOperator.PLUS || c == (char)MathOperator.MINUS;
    }

    private bool DotIsExist() {
      string temp = LblResult.Text;
      int indexPrevOper = IndexPrevOper(temp, temp.Length - 1);
      if (indexPrevOper != -1) {
        temp = temp.Substring(indexPrevOper);
      } else {
        temp = temp.Substring(0);
      }


      return temp.Contains('.');
    }

  }
}
